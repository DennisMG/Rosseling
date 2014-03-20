using System;

namespace MiniTrello.Domain.Entities
{
    public class Sessions : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual Accounts User { get; set; }
        public virtual DateTime LoginDate { get; set; }
        public virtual DateTime ExpireDate { get; set; }
        public virtual int DurationTime { get; set; }
        public virtual string Token { get; set; }

        public virtual bool IsTokenActive()
        {
            var actualTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                          DateTime.Now.Hour,DateTime.Now.Minute, DateTime.Now.Second);
            return ExpireDate>actualTime;
        }
    }
}
