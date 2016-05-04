﻿using System;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

namespace CodeJam.Collections
{
	[TestFixture]
	public class ComparerBuilderTests
	{
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		private class TestClass
		{
			public int    Field1;
			public string Prop2 { get; set; }

			private static int _n;
			private int _field = ++_n;
		}

		[Test]
		public void GetHashCodeTest()
		{
			var eq = ComparerBuilder<TestClass>.GetEqualityComparer();

			Assert.That(eq.GetHashCode(new TestClass()), Is.Not.EqualTo(0));
			Assert.That(eq.GetHashCode(null),            Is.EqualTo(0));
		}

		[Test]
		public void EqualsTest()
		{
			var eq = ComparerBuilder<TestClass>.GetEqualityComparer();

			Assert.That(eq.Equals(new TestClass(), new TestClass()),              Is.True);
			Assert.That(eq.Equals(null,            null),                         Is.True);
			Assert.That(eq.Equals(null,            new TestClass()),              Is.False);
			Assert.That(eq.Equals(new TestClass(), null),                         Is.False);
			Assert.That(eq.Equals(new TestClass(), new TestClass { Field1 = 1 }), Is.False);
		}

		private class NoMemberClass
		{
		}

		[Test]
		public void NoMemberTest()
		{
			var eq = ComparerBuilder<NoMemberClass>.GetEqualityComparer();

			Assert.That(eq.GetHashCode(new NoMemberClass()), Is.Not.EqualTo(0));

			Assert.That(eq.Equals(new NoMemberClass(), new NoMemberClass()), Is.True);
			Assert.That(eq.Equals(new NoMemberClass(), null),                Is.False);
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		private class OneMemberClass
		{
			public int Field1;
		}

		[Test]
		public void OneMemberTest()
		{
			var eq = ComparerBuilder<OneMemberClass>.GetEqualityComparer();

			Assert.That(eq.GetHashCode(new OneMemberClass()), Is.Not.EqualTo(0));

			Assert.That(eq.Equals(new OneMemberClass(), new OneMemberClass()),             Is.True);
			Assert.That(eq.Equals(new OneMemberClass(), null),                             Is.False);
			Assert.That(eq.Equals(new OneMemberClass(), new OneMemberClass { Field1 = 1}), Is.False);
		}

		[Test]
		public void DistinctTest()
		{
			var eq  = ComparerBuilder<TestClass>.GetEqualityComparer();
			var arr = new[]
			{
				new TestClass { Field1 = 1, Prop2 = "2"  },
				new TestClass { Field1 = 1, Prop2 = null },
				null,
				new TestClass { Field1 = 2, Prop2 = "1"  },
				new TestClass { Field1 = 2, Prop2 = "2"  },
				new TestClass { Field1 = 2, Prop2 = "2"  },
				null
			};

			Assert.That(arr.Distinct(eq).Count(), Is.EqualTo(5));
		}

		[Test]
		public void DistinctByMember1Test()
		{
			var eq  = ComparerBuilder<TestClass>.GetEqualityComparer(t => t.Field1);
			var arr = new[]
			{
				new TestClass { Field1 = 1, Prop2 = "2"  },
				new TestClass { Field1 = 1, Prop2 = null },
				null,
				new TestClass { Field1 = 2, Prop2 = "1"  },
				new TestClass { Field1 = 2, Prop2 = "2"  },
				new TestClass { Field1 = 2, Prop2 = "2"  },
				null
			};

			Assert.That(arr.Distinct(eq).Count(), Is.EqualTo(3));
		}

		[Test]
		public void DistinctByMember2Test()
		{
			var eq  = ComparerBuilder<TestClass>.GetEqualityComparer(t => t.Field1, t => t.Prop2);
			var arr = new[]
			{
				new TestClass { Field1 = 1, Prop2 = "2"  },
				new TestClass { Field1 = 1, Prop2 = null },
				null,
				new TestClass { Field1 = 2, Prop2 = "1"  },
				new TestClass { Field1 = 2, Prop2 = "2"  },
				new TestClass { Field1 = 2, Prop2 = "2"  },
				null
			};

			Assert.That(arr.Distinct(eq).Count(), Is.EqualTo(5));
		}
	}
}
