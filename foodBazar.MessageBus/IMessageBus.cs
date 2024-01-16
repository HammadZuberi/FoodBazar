using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foodBazar.MessageBus
{
	public interface IMessageBus
	{
		Task PublishMessage(string Topic_queue_Name, object message);
	}
}
