﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    internal static class SpecimenBuilderNode
    {
        internal static ISpecimenBuilderNode ReplaceNode(
            this ISpecimenBuilderNode graph,
            IEnumerable<ISpecimenBuilder> with,
            Func<ISpecimenBuilderNode, bool> when)
        {
            if (when(graph))
                return graph.Compose(with);

            var nodes = from b in graph
                        let n = b as ISpecimenBuilderNode
                        select n != null ? n.ReplaceNode(with, when) : b;
            return graph.Compose(nodes);
        }

        internal static ISpecimenBuilderNode ReplaceNode(
            this ISpecimenBuilderNode graph,
            ISpecimenBuilderNode with,
            Func<ISpecimenBuilderNode, bool> when)
        {
            if (when(graph))
                return with;

            var nodes = from b in graph
                        let n = b as ISpecimenBuilderNode
                        select n != null ? n.ReplaceNode(with, when) : b;
            return graph.Compose(nodes);
        }

        internal static IEnumerable<ISpecimenBuilderNode> Parents(
            this ISpecimenBuilderNode graph,
            Func<ISpecimenBuilder, bool> predicate)
        {
            foreach (var b in graph)
            {
                if (predicate(b))
                    yield return graph;

                var n = b as ISpecimenBuilderNode;
                if (n != null)
                {
                    foreach (var n1 in n.Parents(predicate))
                        yield return n1;
                }
            }
        }

        internal static IEnumerable<ISpecimenBuilderNode> SelectNodes(
            this ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> predicate)
        {
            if (predicate(graph))
                yield return graph;

            foreach (var b in graph)
            {
                var n = b as ISpecimenBuilderNode;
                if (n != null)
                    foreach (var n1 in n.SelectNodes(predicate))
                        yield return n1;
            }
        }
    }
}
