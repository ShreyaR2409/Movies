﻿using App.Core.Models;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<MovieDto, Movie>();
            CreateMap<UserDto, User>();
        }
    }
}
