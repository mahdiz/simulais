using System;

public class ProgressEventArgs : EventArgs
{
	public long Total { get; set; }
	public long Done { get; set; }
	public string Message { get; set; }

	public ProgressEventArgs(string message, long total, long done)
	{
		Total = total;
		Done = done;
		Message = message;
	}
}