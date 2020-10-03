using Microsoft.AspNetCore.Mvc;
using SendInvite.DTOs;
using SendInvite.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace InviteTests
{
    public class InviteTest
    {
        [Theory]
        [ClassData(typeof(Data))]
        public void ValidationsTest(string message, string [] numbers, ObjectResult expectedResult)
        {
            //Arrange
            var sendSMS = new SendInvites();
            //Act
            var result = sendSMS.Result(numbers, message);
            //Assert
            Assert.Equal(expectedResult.StatusCode, result.StatusCode);
        }
    }

    public class Data : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //Dupl numbers
            yield return new object[] { "Этот текст", new string[] { "79205555552", "79541234569", "79205555552" }, new ObjectResult("") { StatusCode = 404 } };
            //OK
            yield return new object[] { "Этот текст", new string[] { "79205555552", "79541234569", "79205551554" }, new ObjectResult("") { StatusCode = 200 } };
            //Empty string
            yield return new object[] { "", new string[] { "7920555555", "79541234569", "7920555155" }, new ObjectResult("") { StatusCode = 405 } };
            // Null numbers
            yield return new object[] { "Этот текст", new string[] { }, new ObjectResult("") { StatusCode = 401 } };
            //Over 16 numbers
            yield return new object[] { "Этот текст", new string[] {
                "7920551555", "79341234569", "7944555155", "79641254569", "79741134569", "79842234569", "79941234539", "79511244569"
                , "79521234569", "79533254569", "79541734569", "79551234669", "79561234569", "79171234569", "79581224569", "7991534569", "79597418525"
                }, new ObjectResult("") { StatusCode = 402 } };
            //Overflow
            yield return new object[]
            { "Этот текст должен превышать 160 символов транслита потому добавим сюда немного больше текста, просто потому, что это весело",
                new string[] { "79205555552", "79541234569", "79205551557" }, new ObjectResult("") { StatusCode = 407 } };
            //Numbers ! startWith 7
            yield return new object[] { "Этот текст", new string[] { "920555555", "79541234569", "7920555155" }, new ObjectResult("") { StatusCode = 400 } };
            //Numbers lenth ! 11
            yield return new object[] { "Этот текст", new string[] { "7920555555", "79541234569", "79205" }, new ObjectResult("") { StatusCode = 400 } };

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
