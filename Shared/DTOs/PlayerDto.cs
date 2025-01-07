using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class PlayerDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public int Score { get; set; }

        public Guid GameId { get; set; }

        public Guid DeckId { get; set; }

        public bool UnoSaid { get; set; }

        public DateTime SayUnoUntil { get; set; }
    }
}
