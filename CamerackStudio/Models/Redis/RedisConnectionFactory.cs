using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CamerackStudio.Models.Redis
{
    public class RedisConnectionFactory
    {
            private static readonly Lazy<ConnectionMultiplexer> Connection;
            static RedisConnectionFactory()
            {
                Connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost,abortConnect=false"));
        }

            public static ConnectionMultiplexer GetConnection() => Connection.Value;
        }
    }

