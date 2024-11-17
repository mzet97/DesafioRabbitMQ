import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

export let options = {
    stages: [
        { duration: '20s', target: 5000 },     // Aumentar para 1.000 VUs em 30s
        { duration: '20s', target: 5000 },      // Aumentar para 5.000 VUs em 1 minuto
        { duration: '20s', target: 10000 },     // Aumentar para 10.000 VUs em 2 minutos
        { duration: '20s', target: 10000 },     // Manter 10.000 VUs por 2 minutos
        { duration: '10s', target: 0 },         // Reduzir para 0 VUs em 1 minuto
    ],
    thresholds: {
        'http_req_duration': ['p(95)<500'],    // 95% das requisições com duração < 500ms
        'checks': ['rate>0.9'],                // 90% dos checks devem passar
    },
};
// Métrica personalizada para rastrear a taxa de erros
export let errorRate = new Rate('errors');

export default function () {
    let url = 'http://localhost:8080/api/Protocolo/teste';

    let params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let res = http.get(url, params);

    let success = check(res, {
        'status is 200': (r) => r.status === 200,
    });

    errorRate.add(!success);

    sleep(0.01); // Reduz o intervalo de espera para aumentar a taxa de requisições
}
