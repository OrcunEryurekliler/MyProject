using System;
using System.Collections.Generic;

namespace MyProject.Application.Helpers
{
    public static class SlotGenerator
    {
        public static List<TimeSpan> GenerateDailySlots()
        {
            var slots = new List<TimeSpan>();

            var morningStart = new TimeSpan(9, 0, 0);
            var morningEnd = new TimeSpan(12, 30, 0);
            var afternoonStart = new TimeSpan(13, 30, 0);
            var afternoonEnd = new TimeSpan(17, 30, 0);

            var current = morningStart;
            while (current < morningEnd)
            {
                slots.Add(current);
                current = current.Add(TimeSpan.FromMinutes(30));
            }

            current = afternoonStart;
            while (current < afternoonEnd)
            {
                slots.Add(current);
                current = current.Add(TimeSpan.FromMinutes(30));
            }

            return slots;
        }
    }

}
