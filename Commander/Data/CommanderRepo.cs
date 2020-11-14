using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;

namespace Commander.Data
{
    public class CommanderRepo : ICommanderRepo
    {
        private readonly SqLiteContext _context;

        public CommanderRepo(SqLiteContext context)
        {
            _context = context;
        }

        public bool SaveChanges() => _context.SaveChanges() >= 0;

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(s => s.Id == id);
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null) {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.Add(cmd);
        }

        public void UpdateCommand(Command cmd)
        {
            // Nothing
        }
    }
}