namespace PreAdvertScreen
{
	public readonly struct ScreenData
	{
		public readonly string CounterText;
		public readonly string TimerText;
		
		public readonly string Tag;

		public ScreenData(string counterText, string timerText, string tag)
		{
			CounterText = counterText;
			TimerText = timerText;
			Tag = tag;
		}
	}
}