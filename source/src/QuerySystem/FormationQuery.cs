﻿using KdTree;
using KdTree.Math;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace RTSCamera.QuerySystem
{
    public class FormationQuery
    {
        public Formation Formation { get; }

        public QueryData<KdTree<float, AgentPointInfo>> KdTree { get; }

        public Agent NearestAgent(Vec2 position, bool refresh = false)
        {
            if (refresh)
            {
                KdTree.Expire();
            }
            var result = KdTree.Value.GetNearestNeighbours(new float[2] { position.x, position.y }, 1);
            return result.Length == 0 ? null : result[0].Value.Agent;
        }

        public FormationQuery(Formation formation)
        {
            Formation = formation;

            KdTree = new QueryData<KdTree<float, AgentPointInfo>>(() =>
            {
                var tree = new KdTree<float, AgentPointInfo>(2, new FloatMath(), AddDuplicateBehavior.Skip);
                formation.ApplyActionOnEachUnit(agent =>
                {
                    if (agent.IsActive())
                        AddAgent(tree, agent);
                });
                return tree;
            }, 0.23f);
        }

        private static void AddAgent(KdTree<float, AgentPointInfo> tree, Agent agent)
        {
            tree.Add(new float[2] {agent.Position.x, agent.Position.y},
                new AgentPointInfo(agent));
        }

        private static Vec2 Average(KdTreeNode<float, AgentPointInfo>[] points)
        {
            Vec2 result = Vec2.Zero;
            foreach (var point in points)
            {
                result.x += (float)point.Point[0];
                result.y += (float)point.Point[1];
            }

            if (points.Length != 0)
            {
                result *= 1.0f / points.Length;
            }

            return result;
        }

        private static Vec2 Average(ICollection<AgentPointInfo> points)
        {
            Vec2 result = Vec2.Zero;
            foreach (var point in points)
            {
                result.x += (float) point.Point.X;
                result.y += (float) point.Point.Y;
            }

            if (points.Count != 0)
            {
                result *= 1.0f / points.Count;
            }

            return result;
        }

        private static Vec2 Average(IEnumerable<Vec2> points)
        {
            Vec2 result = Vec2.Zero;
            int count = 0;
            foreach (var point in points)
            {
                ++count;
                result += point;
            }

            if (count != 0)
            {
                result *= 1.0f / count;
            }

            return result;
        }
    }
}