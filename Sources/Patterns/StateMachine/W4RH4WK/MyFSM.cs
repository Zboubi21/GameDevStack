// https://gist.github.com/W4RH4WK/5dea8f55532e0526da8b6e60c566c259

using System;

namespace GenericFSM
{

	enum MyStates { Init, Foo, Bar, Empty, Finish };

	class MyFSM : FSM<MyStates>
	{
		public MyFSM() : base(MyStates.Init) { }

		void InitTransition(MyStates prev)
		{
			Console.Out.WriteLine(prev.ToString() + " -> Init");
		}

		void InitState()
		{
			Console.Out.WriteLine("Init State");
			Transition(MyStates.Foo);
		}

		void FooTransition(MyStates prev)
		{
			Console.Out.WriteLine(prev.ToString() + " -> Foo");
		}

		void FooState()
		{
			Console.Out.WriteLine("Foo State");
			Transition(MyStates.Bar);
		}

		// Simply omit empty transitions

		void BarState()
		{
			Console.Out.WriteLine("Bar State");
			Transition(MyStates.Finish);
		}

		// Simply omit empty states

		void FinishTransition(MyStates prev)
		{
			Console.Out.WriteLine(prev.ToString() + " -> Finish");
			Transition(MyStates.Bar);
		}

		void FinishState()
		{
			Console.Out.WriteLine("Finish State");
			Environment.Exit(0);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			MyFSM fsm = new MyFSM();

			// Trigger first transition manually (if needed)
			fsm.Transition(MyStates.Init);

			// Have Fun
			while (true)
			{
				fsm.StateDo();
			}
		}
	}
}