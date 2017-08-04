using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Threading.Tasks;

namespace GameCore.Specs
{
    [Binding]
    public class Hooks : Steps
    {
        [BeforeScenario("elf")]
        public void BeforeScenario()
        {

        }

        [AfterScenario]
        public void AfterScenario()
        {

        }
    }
}
