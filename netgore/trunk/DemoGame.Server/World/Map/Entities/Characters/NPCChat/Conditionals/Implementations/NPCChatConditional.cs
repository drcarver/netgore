using System.Diagnostics;
using System.Linq;
using NetGore.NPCChat.Conditionals;

namespace DemoGame.Server.NPCChat.Conditionals
{
    /// <summary>
    /// The base class for a conditional used in the NPC chatting. Each instanceable derived class
    /// must include a parameterless constructor (preferably private).
    /// </summary>
    public abstract class NPCChatConditional : NPCChatConditionalBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditional"/> class.
        /// </summary>
        /// <param name="name">The unique display name of this NPCChatConditionalBase. This name must be unique
        /// for each derived class type. This string is case-sensitive.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        protected NPCChatConditional(string name, params NPCChatConditionalParameterType[] parameterTypes)
            : base(name, parameterTypes)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional evaluation.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use.</param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected abstract bool DoEvaluate(User user, NPC npc, NPCChatConditionalParameter[] parameters);

        /// <summary>
        /// When overridden in the derived class, performs the actual conditional evaluation.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="npc">The NPC.</param>
        /// <param name="parameters">The parameters to use. </param>
        /// <returns>True if the conditional returns true for the given <paramref name="user"/>,
        /// <paramref name="npc"/>, and <paramref name="parameters"/>; otherwise false.</returns>
        protected override bool DoEvaluate(object user, object npc, NPCChatConditionalParameter[] parameters)
        {
            return DoEvaluate((User)user, (NPC)npc, parameters);
        }

        /// <summary>
        /// Safely gets the percent of two values.
        /// </summary>
        /// <param name="n">The numerator.</param>
        /// <param name="d">The denominator.</param>
        /// <returns>The percent of n/d.</returns>
        protected static float GetPercent(uint n, uint d)
        {
            Debug.Assert(n >= float.MinValue && n <= float.MaxValue);
            Debug.Assert(d >= float.MinValue && d <= float.MaxValue);
            return GetPercent((float)n, d);
        }

        /// <summary>
        /// Safely gets the percent of two values.
        /// </summary>
        /// <param name="n">The numerator.</param>
        /// <param name="d">The denominator.</param>
        /// <returns>The percent of n/d.</returns>
        protected static float GetPercent(int n, int d)
        {
            Debug.Assert(n >= float.MinValue && n <= float.MaxValue);
            Debug.Assert(d >= float.MinValue && d <= float.MaxValue);
            return GetPercent((float)n, d);
        }

        /// <summary>
        /// Safely gets the percent of two values.
        /// </summary>
        /// <param name="n">The numerator.</param>
        /// <param name="d">The denominator.</param>
        /// <returns>The percent of n/d.</returns>
        protected static float GetPercent(float n, float d)
        {
            // Ensure we get absolute values for 0% and 100%
            if (n == 0 || d == 0)
                return 0;
            if (n == d)
                return 100;

            var p = (n / d) * 100.0f;

            Debug.Assert(p >= 0f);
            Debug.Assert(p <= 100f);

            return p;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}