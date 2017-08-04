using System;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;

namespace GameCore.Specs
{
    [Binding]
    public class PlayerCharacterSteps
    {
        private readonly PlayerCharacterStepsContext _context;
        public PlayerCharacterSteps(PlayerCharacterStepsContext context)
        {
            _context = context;
        }

        [When ("I take (.*) damage")]
        public void WhenItakeDamage(int damage)
        {
            _context.Player.Hit(damage);
        }

        // Scoped binding - this step will only run for scenarios with @elf tag
        [When("I take (.*) damage")]
        [Scope(Tag = "elf")]
        public void WhenItakeDamageAsAnElf(int damage)
        {
            _context.Player.Hit(damage);
        }


        [Then(@"My health should now be (.*)")]
        public void ThenMyHealthShouldNowBe(int expectedHealth)
        {
            Assert.Equal(expectedHealth, _context.Player.Health);
        }

        [Then(@"I should be dead")]
        public void ThenIShouldBeDead()
        {
            Assert.Equal(0, _context.Player.Health);
            Assert.True(_context.Player.IsDead);
        }

        [Given(@"I have a damage resistance of (.*)")]
        public void GivenIHaveADamageResistanceOf(int damageResistance)
        {
            _context.Player.DamageResistance = damageResistance;
        }

        [Given(@"I'm an Elf")]
        public void GivenIMAnElf()
        {
            _context.Player.Race = "Elf";
        }

        [Given(@"I have the following attributes")]
        public void GivenIHaveTheFollowingAttributes(Table table)
        {
            // Pulling attributes directly from feature file (weakly typed)
            //var race = table.Rows.First(TableRow => TableRow["attribute"] == "Race")["value"];
            //var resistance =  table.Rows.First(TableRow => TableRow["attribute"] == "Resistance")["value"];

            // Pulling attributes from feature and mapping them to existing attribute class (strongly typed)
            //var attributes = table.CreateInstance<PlayerAttributes>();

            // With dynamic attributes
            dynamic attributes = table.CreateDynamicInstance();

            _context.Player.Race = attributes.Race;
            _context.Player.DamageResistance = attributes.Resistance;
        }

        [Given(@"My character class is set to (.*)")]
        public void GivenMyCharacterClassIsSetToHealer(CharacterClass characterClass)
        {
            _context.Player.CharacterCLass = characterClass;
        }

        [When(@"Cast a healing spell")]
        public void WhenCastAHealingSpell()
        {
            _context.Player.CastHealingSpell();
        }

        [Given(@"I have the following magical items")]
        public void GivenIHaveTheFollowingMagicalItems(Table table)
        {
            /* Weakly typed version
            foreach (var row in table.Rows)
            {
                var name = row["item"];
                var value = row["value"];
                var power = row["power"];

                _context.Player.MagicalItems.Add(new MagicalItem
                {
                    Name = name,
                    Value = int.Parse(value),
                    Power = int.Parse(power)
                });
            } */

            /* Strongly typed version
            IEnumerable<MagicalItem> items = table.CreateSet<MagicalItem>();
            _context.Player.MagicalItems.AddRange(items); */

            //Dynamic version
            IEnumerable<dynamic> items = table.CreateDynamicSet();

            foreach (var magicalItem in items)
            {
                _context.Player.MagicalItems.Add(new MagicalItem
                {
                    Name = magicalItem.name,
                    Value = magicalItem.value,
                    Power = magicalItem.power
                });
            }

        }

        [Then(@"My total magical power should be (.*)")]
        public void ThenMyTotalMagicalPowerShouldBe(int expectedPower)
        {
            Assert.Equal(expectedPower, _context.Player.MagicalPower);
        }

        //Custom data transform w/ regular expression
        [Given(@"I last slept (.* days ago)")]
        public void GivenILastSleptDaysAgo(DateTime lastSlept)
        {
            _context.Player.LastSleepTime = lastSlept;
        }

        [When(@"I read a restore health scroll")]
        public void WhenIReadARestoreHealthScroll()
        {
            _context.Player.ReadHealthScroll();
        }

        [Given(@"I have the following weapons")]
        public void GivenIHaveTheFollowingWeapons(IEnumerable<Weapon> weapons)
        {
            _context.Player.Weapons.AddRange(weapons);
        }

        //Custom data transform no regular expression
        [Then(@"My weapons should be worth (.*)")]
        public void ThenMyWeaponsShouldBeWorth(int value)
        {
            Assert.Equal(value, _context.Player.WeaponsValue);
        }

        // Context injection - passing power value between steps
        [Given(@"I have an Amulet with a power of (.*)")]
        public void GivenIHaveAnAmuletWithAPowerOf(int power)
        {
            _context.Player.MagicalItems.Add(
                new MagicalItem
                {
                    Name = "Amulet",
                    Power = power
                });

            _context.StartingMagicalPower = power;
        }

        // Context injection - passing power value between steps
        [When(@"I use a magical Amulet")]
        public void WhenIUseAMagicalAmulet()
        {
            _context.Player.UseMagicalItem("Amulet"); 
        }

        // Context injection - passing power value between steps
        [Then(@"The Amulet power should not be reduced")]
        public void ThenTheAmuletPowerShouldNotBeReduced()
        {
            int expectedPower;

            expectedPower = _context.StartingMagicalPower;

            Assert.Equal(expectedPower, _context.Player.MagicalItems.First(item => item.Name == "Amulet").Power);
        }


    }
}
