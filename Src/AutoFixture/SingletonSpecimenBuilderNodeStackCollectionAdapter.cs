﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Collections.ObjectModel;

namespace Ploeh.AutoFixture
{
    public class SingletonSpecimenBuilderNodeStackCollectionAdapter : Collection<ISpecimenBuilderTransformation>
    {
        private ISpecimenBuilderNode graph;
        private readonly Func<ISpecimenBuilderNode, bool> isWrappedGraph;

        public SingletonSpecimenBuilderNodeStackCollectionAdapter(
            ISpecimenBuilderNode graph,
            Func<ISpecimenBuilderNode, bool> wrappedGraphPredicate,
            params ISpecimenBuilderTransformation[] transformations)
        {
            this.graph = graph;
            this.isWrappedGraph = wrappedGraphPredicate;

            foreach (var t in transformations)
            {
                base.Add(t);
            }
        }

        public event EventHandler<SpecimenBuilderNodeEventArgs> GraphChanged;

        public ISpecimenBuilderNode Graph
        {
            get { return this.graph; }
        }

        protected override void ClearItems()
        {
            var wasNotEmpty = this.Count > 0;
            base.ClearItems();
            if (wasNotEmpty)
                this.UpdateGraph();
        }

        protected override void InsertItem(int index, ISpecimenBuilderTransformation item)
        {
            base.InsertItem(index, item);
            this.UpdateGraph();            
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            this.UpdateGraph();
        }

        protected override void SetItem(int index, ISpecimenBuilderTransformation item)
        {
            base.SetItem(index, item);
            this.UpdateGraph();
        }

        protected virtual void OnGraphChanged(SpecimenBuilderNodeEventArgs e)
        {
            var handler = this.GraphChanged;
            if (handler != null)
                handler(this, e);
        }

        private void UpdateGraph()
        {
            ISpecimenBuilder g = this.graph.SelectNodes(this.isWrappedGraph).First();
            var builder = this.Aggregate(g, (b, t) => t.Transform(b));

            var node = builder as ISpecimenBuilderNode;
            if (node == null)
                throw new InvalidOperationException("An ISpecimenBuilderTransformation returned a result which cannot be converted to an ISpecimenBuilderNode. To be used in the current context, all ISpecimenBuilderTransformation Transform methods must return an ISpecimenBuilderNode instance.");

            this.graph = node;

            this.OnGraphChanged(new SpecimenBuilderNodeEventArgs(this.graph));
        }
    }
}
