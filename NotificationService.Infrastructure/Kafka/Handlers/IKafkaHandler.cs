﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Kafka.Handlers
{
    public interface IKafkaHandler<T>
    {
        Task HandleAsync(T message);

    }
}
