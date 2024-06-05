﻿using Tourplanner.Entities.Tours;
using Tourplanner.Models;

namespace Tourplanner.DTOs
{
    public class CreateTourDto
    {
        public CreateTourDto(
            string name,
            string description,
            string from,
            string to,
            Coordinates coordinates,
            float estimatedTime,
            TransportType transportType
        )
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            Coordinates = coordinates;
            EstimatedTime = estimatedTime;
            TransportType = transportType;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public Coordinates Coordinates { get; set; }
        public float EstimatedTime { get; set; }
        public TransportType TransportType { get; set; }
    }
}