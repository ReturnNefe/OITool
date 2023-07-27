namespace OITool.Interface.Judge
{
    /// <summary>
    /// The status of <see cref="IJudger.Judge" />.
    /// </summary>
    public enum JudgerStatus
    {
        /// <summary/>
        Accepted,
        
        /// <summary/>
        WrongAnswer,
        
        /// <summary/>
        TimeLimitExceed,
        
        /// <summary/>
        MemoryLimitExceed,
        
        /// <summary/>
        RuntimeError
    }
}