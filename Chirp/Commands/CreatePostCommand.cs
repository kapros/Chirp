﻿using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Responses;
using Chirp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Commands
{
    public class CreatePostCommand : Command, IRequest<Accepted>
    {
        public CreatePostCommand(UserId userId) : base(userId)
        {
        }

        public CreatePostRequest CreatePostRequest { get; set; }

        public string CreatedBy { get; set; }
    }
}
