Imports Logger.Data
Imports System

Namespace Output
	Public Interface ILogOutput
        Property AlwaysLog As Boolean
        Property LogPreviousExceptions As Boolean
        Property ResetTime As Boolean
		Sub LogMessage(ByVal log As Log)
	End Interface
End Namespace
