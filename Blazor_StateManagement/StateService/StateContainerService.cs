using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor_StateManagement.StateService
{
	public class StateContainerService
	{
		public string Message { get; set; } = "I am the Initial Value";

		public int Value { get; set; } = 0;

		public event Action OnStateChange;

		public void SetMessage(string value)
		{
			Message = value;
			NotifyStateChanged();
		}

		public void SetValue(int value)
		{
			Value = value;
			NotifyStateChanged();
		}


		private void NotifyStateChanged() => OnStateChange?.Invoke();
	}
}
