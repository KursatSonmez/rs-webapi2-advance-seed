using RS.Core.Domain;
using System;
using System.Collections.Generic;

namespace RS.Core.Testing.Arrange
{
    public static class AutoCodeDtos
    {
        public static List<AutoCode> InMemoryList()
        {
            var autoCodeData = new List<AutoCode>
            {
                new AutoCode
                {
                    ID = Guid.Parse("1e5757c3-2f69-4c5f-918d-2eaa1b478850"),
                    CodeFormat="CF-{0}-UT",
                    LastCodeNumber=142,
                    ScreenCode="TSC01",
                    CreateBy=Guid.Parse("ae9569bf-3e20-4f6a-a930-a70066f8ceb8"),
                    CreateDT=new DateTime(2017,10,03)
                },

                new AutoCode
                {
                    ID = Guid.Parse("ce27647e-8c2f-4aff-8558-eb0337ec59ce"),
                    CodeFormat="UT-AY--{0}",
                    LastCodeNumber=1903,
                    ScreenCode="TSC02",
                    CreateBy=Guid.Parse("ae9569bf-3e20-4f6a-a930-a70066f8ceb8"),
                    CreateDT=new DateTime(2017,10,02)
                  }
            };

            return autoCodeData;
        }
        public static string ScreenCodeForAutomaticCodeGenerationTest= "TSC01";
        public static string IncorrectCodeFormat = "CF-TS[9}";
        public static string CorrectCodeFormat = "CF-TS{0}";
        public static string IncorrectScreenCode = "XXX-01";
    }
}