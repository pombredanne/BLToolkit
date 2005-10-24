using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder
{
	[TestFixture]
	public class LazyInstance
	{
		public LazyInstance()
		{
			TypeFactory.SaveTypes = true;
		}

		public class TestField
		{
			public TestField()
			{
				Value = 10;
			}

			public TestField(int p1, float p2)
			{
				Value = p1 + (int)p2;
			}

			public TestField(TestField p1)
			{
				Value = 77;
			}

			public int Value;
		}

		public abstract class Object1
		{
			[LazyInstanceAttribute]
			public abstract ArrayList List { get; set; }

			[LazyInstanceAttribute]
			public abstract string    Str { get; set; }

			[LazyInstanceAttribute]
			public abstract string this[int i] { get; set; }

			[LazyInstanceAttribute]
			public abstract TestField Field { get; set; }
		}

		[Test]
		public void NoParamTest()
		{
			BuildContext context = TypeFactory.GetType(typeof(Object1));

			Console.WriteLine(context.Type.Type);

			Object1 o = (Object1)Activator.CreateInstance(context.Type);

			Assert.IsNotNull(o.List);
			Assert.AreEqual("", o.Str);
			Assert.AreEqual(10, o.Field.Value);
		}

		[AttributeUsage(AttributeTargets.Property)]
		public class TestParameterAttribute : ParameterAttribute
		{
			public TestParameterAttribute()
				: base(new TestField())
			{
			}
		}

		public abstract class Object2
		{
			[LazyInstanceAttribute, Parameter(10)]
			public abstract ArrayList List { get; set; }

			[LazyInstanceAttribute, Parameter("test")]
			public abstract string Str { get; set; }

			[LazyInstanceAttribute, Parameter(20)]
			public abstract string this[int i] { get; set; }

			[LazyInstanceAttribute, Parameter(20, 30)]
			public abstract TestField Field1 { get; set; }

			[LazyInstanceAttribute, TestParameter]
			public abstract TestField Field2 { get; set; }
		}

		[Test]
		public void ParamTest()
		{
			BuildContext context = TypeFactory.GetType(typeof(Object2));

			Console.WriteLine(context.Type.Type);

			Object2 o = (Object2)Activator.CreateInstance(context.Type);

			Assert.AreEqual(10,     o.List.Capacity);
			Assert.AreEqual("test", o.Str);
			Assert.AreEqual(50,     o.Field1.Value);
			Assert.AreEqual(77,     o.Field2.Value);
		}
	}
}