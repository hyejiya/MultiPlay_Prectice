using System;

namespace MP.Singleton
{
    public abstract class SingletonBase<T>
        where T : SingletonBase<T>
    {
        public static T instance
        {
            get
            {
                if (s_instance == null)
                {
                    //생성자 가져오는 방법
                    //1. System.Reflection 사용
                    //ConstructorInfo constructorInfo =  typeof(T).GetConstructor(new Type[] {}); //타입 타입의 객체 생성자를 가져온다 
                    //constructorInfo.Invoke(null);

                    //2. Activator 사용 
                    s_instance = (T)Activator.CreateInstance(typeof(T));
                }
                return s_instance;
            }
        }

        private static T s_instance;
    }
}
