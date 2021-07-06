using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SuperLocker.Core.Command;

namespace SuperLocker.Api.Models
{
    public sealed class LockCommadRequest
    {
        public Guid LockId { get; set; }
        public Guid UserId { get; set; }
    }
}