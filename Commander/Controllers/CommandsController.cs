using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        public ActionResult<CommandReadDto> GetCommandById([FromRoute] int id)
        {
            Command commandModel = _repo.GetCommandById(id);
            if (commandModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(commandModel));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand([FromBody] CommandCreateDto cmd)
        {
            Command commandModel = _mapper.Map<Command>(cmd);

            _repo.CreateCommand(commandModel);
            _repo.SaveChanges();

            CommandReadDto commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new {commandReadDto.Id}, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand([FromRoute] int id, [FromBody] CommandUpdateDto cmd)
        {
            Command commandModel = _repo.GetCommandById(id);
            if (commandModel == null)
            {
                return NotFound();
            }

            _mapper.Map(cmd, commandModel);
            _repo.UpdateCommand(commandModel);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            Command commandModel = _repo.GetCommandById(id);
            if (commandModel == null)
            {
                return NotFound();
            }

            CommandUpdateDto commandToPatch = _mapper.Map<CommandUpdateDto>(commandModel);

            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModel);
            _repo.UpdateCommand(commandModel);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommandById([FromRoute] int id)
        {
            Command commandModel = _repo.GetCommandById(id);
            if (commandModel == null)
            {
                return NotFound();
            }

            _repo.DeleteCommand(commandModel);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}