using System;
using Whisper.Core.Models.Generation;

namespace Whisper.Modules.NumberGenerator
{
    public sealed class NumberGenerator : ContentGeneratorBase<NumberContent, NumberGeneratorConfiguration>
    {
        public override Guid Id => Guid.Parse("59EEA92F-4D8D-47C1-870B-938AB83B37B5");
        public override string Name => "Number";
        public override string Description => "Generates Numbers.";
        
        private Random _random = new Random();

        protected override NumberContent Create(NumberGeneratorConfiguration configuration)
        {
            int randomValue = _random.Next(0, 10);;

            return new NumberContent(randomValue.ToString());
        }
    }
}
