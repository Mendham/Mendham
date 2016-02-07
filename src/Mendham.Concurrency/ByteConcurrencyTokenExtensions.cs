﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
    public static class ByteConcurrencyTokenExtensions
    {
        public static T SetByteConcurrencyToken<T>(this T obj, byte[] tokenBytes)
           where T : IHasConcurrencyToken
        {
            tokenBytes.VerifyArgumentNotNullOrEmpty(nameof(tokenBytes), "Bytes for token are required");

            IConcurrencyToken token = new ByteConcurrencyToken(tokenBytes);

            return obj.SetConcurrencyToken(token);
        }
    }
}
