import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

export let options = {
    stages: [
        // { duration: '1s', target: 1000 }, 
        { duration: '1m', target: 100000 },
        // { duration: '1m', target: 100000 },
    ],
    thresholds: {
        'http_req_duration': ['p(95)<100000'], // 95% das requisições devem ter duração < 500ms
        'checks': ['rate>0.9'],            // Mais de 90% dos checks devem passar
    },
};

// Métrica personalizada para rastrear a taxa de erros
export let errorRate = new Rate('errors');

export default function () {
    // URL da API
    let url = 'http://localhost:8080/api/Protocolo/teste'; // Substitua pela URL da sua API

    // Cabeçalhos (se necessário)
    let params = {
        headers: {
            'Content-Type': 'application/json',
            // Adicione outros cabeçalhos conforme necessário, como autenticação
        },
    };

    // Corpo da requisição (se aplicável)
    // let payload = JSON.stringify({
    //     key1: 'valor1',
    //     key2: 'valor2',
    // });

    // Enviar a requisição POST (use GET, PUT, DELETE conforme necessário)
    let res = http.get(url, params);

    // Verificações
    let success = check(res, {
        'status is 200': (r) => r.status === 200,
        // 'response time < 500ms': (r) => r.timings.duration < 500,
        // Adicione outras verificações conforme necessário
    });

    // Registrar erros
    errorRate.add(!success);

    // Pausa entre as requisições
    sleep(1);
}
