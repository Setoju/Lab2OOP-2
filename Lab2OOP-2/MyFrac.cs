using System;

namespace Program
{
    public class MyTime
    {
        private int _hour;
        private int _minute;
        private int _second;

        public MyTime(int h, int m, int s)
        {
            SetTime(h, m, s);
        }

        private void SetTime(int h, int m, int s)
        {
            if (s % 60 != 0 || (s % 60 == 0 && (s / 60 >= 1 || s / 60 <= -1)))
            {
                m += s / 60;
                s %= 60;
            }
            if (m % 60 != 0 || (m % 60 == 0 && (m / 60 >= 1 || m / 60 <= -1)))
            {
                h += m / 60;
                m %= 60;
            }
            if (h > 23 || h < -23)
            {
                h %= 24;
            }
            _hour = h;
            _minute = m;
            _second = s;            
        }

        public override string ToString()
        {
            return $"{_hour:D2}:{_minute:D2}:{_second:D2}";
        }
        
        static public MyTime TimeSinceMidnight(int t)
        {
            int secPerDay = 60 * 60 * 24;
            t %= secPerDay;
            if (t < 0)
            {
                t += secPerDay;
            }
            return new MyTime(t / 3600, (t / 60) % 60, t % 60);
        }

        static public int TimeSinceMidnight(MyTime t)
        {
            return t._hour * 3600 + t._minute * 60 + t._second;
        }

        public void AddOneSecond()
        {
            int totalSeconds = TimeSinceMidnight(this) + 1;

            SetTime(totalSeconds / 3600, (totalSeconds / 60) % 60, totalSeconds % 60);
        }

        public void AddOneMinute()
        {
            int totalSeconds = TimeSinceMidnight(this) + 60;

            SetTime(totalSeconds / 3600, (totalSeconds / 60) % 60, totalSeconds % 60);
        }

        public void AddOneHour()
        {
            int totalSeconds = TimeSinceMidnight(this) + 3600;

            SetTime(totalSeconds / 3600, (totalSeconds / 60) % 60, totalSeconds % 60);
        }

        public void AddSeconds(int s)
        {
            int totalSeconds = TimeSinceMidnight(this) + s;
            totalSeconds = (totalSeconds % 86400 + 86400) % 86400;

            SetTime(totalSeconds / 3600, (totalSeconds / 60) % 60, totalSeconds % 60);
        }

        static public int Difference(MyTime otherTime, MyTime thisTime)
        {
            int diff = Math.Abs(TimeSinceMidnight(otherTime) - TimeSinceMidnight(thisTime));
            
            return diff;
        }

        static public string WhatLesson(MyTime mt)
        {
            int timeInSec = TimeSinceMidnight(mt);
            TimeSpan time = TimeSpan.FromHours(mt._hour) + TimeSpan.FromMinutes(mt._minute) + TimeSpan.FromSeconds(mt._second);
            List<(TimeSpan start, TimeSpan end)> lessons = new List<(TimeSpan, TimeSpan)>{
            (TimeSpan.FromHours(8), TimeSpan.FromHours(9)+TimeSpan.FromMinutes(20)),
            (TimeSpan.FromHours(9)+TimeSpan.FromMinutes(40), TimeSpan.FromHours(11)),
            (TimeSpan.FromHours(11)+TimeSpan.FromMinutes(20), TimeSpan.FromHours(12)+TimeSpan.FromMinutes(40)),
            (TimeSpan.FromHours(13), TimeSpan.FromHours(14)+TimeSpan.FromMinutes(20)),
            (TimeSpan.FromHours(14)+TimeSpan.FromMinutes(40), TimeSpan.FromHours(16))
            };

            if (timeInSec >= 28800 && timeInSec <= 57600)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (time >= lessons[i].start && time <= lessons[i].end)
                    {
                        return $"It's the {i + 1} lesson.";                        
                    }
                    else if(i < 4 && time >= lessons[i].end && time <= lessons[i + 1].start)
                    {
                        return $"It's the {i + 1} break.";
                    }
                }
            }
            else if(timeInSec >= 57600)
            {
                return "Uni's over for the day.";
            }
            return "Lessons haven't started yet.";
        }
    }
}