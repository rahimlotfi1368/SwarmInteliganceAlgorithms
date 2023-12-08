using Algorithms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class PSO
    {
        private readonly int _numParticles;
        private readonly int _maxIterations;
        private readonly double _inertia;
        private readonly double _cognitive;
        private readonly double _social;
        private readonly Random _random;

        public PSO(int numParticles, int maxIterations, double inertia, double cognitive, double social)
        {
            _numParticles = numParticles;
            _maxIterations = maxIterations;
            _inertia = inertia;
            _cognitive = cognitive;
            _social = social;
            _random = new Random();
        }

        public double[] Solve(Func<double[], double> fitnessFunction, int numDimensions, double[] minX, double[] maxX)
        {
            var swarm = new Particle[_numParticles];
            var globalBestPosition = new double[numDimensions];
            var globalBestFitness = double.MaxValue;
            var r1 = new double[numDimensions];
            var r2 = new double[numDimensions];

            //Initilizing Population
            for (var i = 0; i < _numParticles; i++)
            {
                swarm[i] = new Particle
                {
                    Position = new double[numDimensions],
                    Velocity = new double[numDimensions],
                    BestPosition = new double[numDimensions],
                    BestFitness = double.MaxValue
                };

                for (var j = 0; j < numDimensions; j++)
                {
                    swarm[i].Position[j] = _random.NextDouble() * (maxX[j] - minX[j]) + minX[j];
                    swarm[i].Velocity[j] = _random.NextDouble() * (maxX[j] - minX[j]) + minX[j];
                    swarm[i].BestPosition[j] = swarm[i].Position[j];
                }

                swarm[i].BestFitness = fitnessFunction(swarm[i].BestPosition);

                if (swarm[i].BestFitness < globalBestFitness)
                {
                    globalBestFitness = swarm[i].BestFitness;
                    swarm[i].BestPosition.CopyTo(globalBestPosition, 0);
                }
            }

            //PSO Main Loop
            for (var iteration = 0; iteration < _maxIterations; iteration++)
            {
                for (var i = 0; i < _numParticles; i++)
                {
                    for (var j = 0; j < numDimensions; j++)
                    {
                        r1[j] = _random.NextDouble();
                        r2[j] = _random.NextDouble();

                        swarm[i].Velocity[j] = _inertia * swarm[i].Velocity[j] +
                                               _cognitive * r1[j] * (swarm[i].BestPosition[j] - swarm[i].Position[j]) +
                                               _social * r2[j] * (globalBestPosition[j] - swarm[i].Position[j]);

                        swarm[i].Position[j] += swarm[i].Velocity[j];

                        if (swarm[i].Position[j] < minX[j])
                        {
                            swarm[i].Position[j] = minX[j];
                            swarm[i].Velocity[j] = 0;
                        }

                        if (swarm[i].Position[j] > maxX[j])
                        {
                            swarm[i].Position[j] = maxX[j];
                            swarm[i].Velocity[j] = 0;
                        }
                    }

                    var fitness = fitnessFunction(swarm[i].Position);

                    if (fitness < swarm[i].BestFitness)
                    {
                        swarm[i].BestFitness = fitness;
                        swarm[i].Position.CopyTo(swarm[i].BestPosition, 0);

                        if (swarm[i].BestFitness < globalBestFitness)
                        {
                            globalBestFitness = swarm[i].BestFitness;
                            swarm[i].BestPosition.CopyTo(globalBestPosition, 0);
                        }
                    }
                }
            }

            return globalBestPosition;
        }
    }
}
