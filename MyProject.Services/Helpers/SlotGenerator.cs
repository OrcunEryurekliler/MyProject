using System;
using System.Collections.Generic;

namespace MyProject.Application.Helpers
{
    public static class SlotGenerator
    {
        public static List<TimeOnly> GenerateDailySlots()
        {
            var slots = new List<TimeOnly>();

            var morningStart = new TimeOnly(9, 0);
            var morningEnd = new TimeOnly(12, 30);
            var afternoonStart = new TimeOnly(13, 30);
            var afternoonEnd = new TimeOnly(17, 30);

            var current = morningStart;
            while (current < morningEnd)
            {
                slots.Add(current);
                current = current.AddMinutes(30);
            }

            current = afternoonStart;
            while (current < afternoonEnd)
            {
                slots.Add(current);
                current = current.AddMinutes(30);
            }

            return slots;
        }
    }
}
