using AutoMapper;
using Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
internal class EmployeeService : IEmployeeService {
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger , IMapper mapper) {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
}

