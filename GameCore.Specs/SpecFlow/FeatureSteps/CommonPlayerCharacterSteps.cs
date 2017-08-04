using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace GameCore.Specs
{
    [Binding]
    public class CommonPlayerCharacterSteps
    {
        private readonly PlayerCharacterStepsContext _context;
        public CommonPlayerCharacterSteps(PlayerCharacterStepsContext context)
        {
            _context = context;
        }


        [Given(@"I'm a new player")]
        public void GivenImANewPlayer()
        {
            _context.Player = new PlayerCharacter();
        }
    }
}
