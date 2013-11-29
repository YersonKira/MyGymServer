using MyGym.Common;
using MyGym.Common.Enum;
using MyGym.Data;
using MyGym.Data.Entities;
using MyGym.Service.Controllers.API.ErrorHandler;
using MyGym.Service.Models.APIHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGym.Service.Models
{
    public class ExerciseRepository
    {
        public Object Get(int exerciseID)
        {
            var exercise = toSerializable(MyGymContext.DB.Ejercicio.Find(exerciseID));
            return exercise;
        }

        private object toSerializable(Ejercicio ejercicio)
        {
            UserExercise exercise = new UserExercise()
            {
                ExerciseID = ejercicio.EjercicioID,
                Name = ejercicio.Nombre,
                Distance = ejercicio.Distancia,
                Duration = ejercicio.Duracion,
                Repetitons = ejercicio.Repeticiones,
                Sets = ejercicio.Sets,
                Type = ejercicio.Tipo.ToString(),
                Weight = ejercicio.Peso,
                Instructions = (from x in ejercicio.Instruccion select new Instruction() { Content = x.Content, Number = x.Step }).ToList()
            };
            return APIFunctions.SuccessResult(exercise, JsonMessage.Success);
        }
        /// <summary>
        /// Retorna los ejercicios por un tipo
        /// </summary>
        /// <param name="type">es el tipo de ejercicio</param>
        /// <returns></returns>
        public IEnumerable<Ejercicio> GetByType(TipoEjercicio type)
        {
            return from x in MyGymContext.DB.Ejercicio.ToList() where x.Tipo.Equals(type) select x;
        }
        Random random = new Random();
        public Ejercicio GetRandomExercise(int tipo)
        {
            if (tipo == 0)
            {
                var ejercicios = GetByType(TipoEjercicio.Cardio).ToList();
                return ejercicios[random.Next(ejercicios.Count)];
            }
            if (tipo == 1)
            {
                var ejercicios = GetByType(TipoEjercicio.Gimnastics).ToList();
                return ejercicios[random.Next(ejercicios.Count)];
            }
            if (tipo == 2)
            {
                var ejercicios = GetByType(TipoEjercicio.Weights).ToList();
                return ejercicios[random.Next(ejercicios.Count)];
            }
            return null;
        }
    }
}