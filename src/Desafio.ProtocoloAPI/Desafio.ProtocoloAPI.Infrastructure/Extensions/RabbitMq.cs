﻿namespace Desafio.ProtocoloAPI.Infrastructure.Extensions;

public class RabbitMq
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}