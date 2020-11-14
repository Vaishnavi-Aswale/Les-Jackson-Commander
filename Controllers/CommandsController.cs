using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            IEnumerable<Command> commandModels = _repo.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandModels));
        }

        [HttpGet("{id}", Name = nameof(GetCommandById))]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            Command commandModel = _repo.GetCommandById(id);

            if (commandModel != null) {
                return Ok(_mapper.Map<CommandReadDto>(commandModel));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto cmd)
        {
            Command commandModel = _mapper.Map<Command>(cmd);

            _repo.CreateCommand(commandModel);
            _repo.SaveChanges();

            CommandReadDto commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new {commandReadDto.Id}, commandReadDto);
        }
    }
}