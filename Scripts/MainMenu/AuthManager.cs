using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;


public class AuthManager : MonoBehaviour
{
    [SerializeField] private InputField emailField; //로그인창 이메일 입력칸
    [SerializeField] private InputField passwordField; // 로그인창 비밀번호 입력칸

    [SerializeField] private InputField emailFieldSignUp; // 회원가입 이메일 입력칸
    [SerializeField] private InputField passwordFieldSignUp; // 회원가입 비밀번호 입력칸
    [SerializeField] private InputField passwordCheckField; // 비밀번호 확인란

    [SerializeField] private GameObject popUp; // 팝업창
    [SerializeField] private Text popUpText;

    private Button loginBtn;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    public static FirebaseUser user;

    public bool IsFirebaseReady { get; private set; } // 파이어베이스의 준비 상태를 체크
    public bool IsLoginOnProgress { get; private set; } // 로그인이 실행 중인지 체크
   

    // Start is called before the first frame update
    void Start()
    {
        loginBtn = GameObject.Find("LoginBtn").GetComponent<Button>();       
        loginBtn.interactable = false; // 로그인 버튼 비활성화

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {   // 파이어베이스를 구동할 수 있는 상태인지 체크
            var result = task.Result;

            if (result != DependencyStatus.Available)
            {
                SetPopUp("파이어베이스 오류");
                IsFirebaseReady = false;
            }

            else
            {
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
            } // 구동할 수 있는 상태라 판단되면 파이어베이스의 auth 기능들을 가져온다.
            
            loginBtn.interactable = IsFirebaseReady; // 로그인 버튼을 활성화
        }
        );   
    }

    public void Login() // 로그인 함수 
    {
        if (!IsFirebaseReady || IsLoginOnProgress || user != null)
        {
            SetPopUp("로그인이 실행 중입니다");
            return;
        } // 파이어베이스가 준비되지 않거나, 로그인이 실행중일 경우 즉시 return

        IsLoginOnProgress = true;
        loginBtn.interactable = false; // 로그인 실행 상태에 돌입하면 로그인 버튼 비활성화  

        // 로그인 실행 메소드
        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(
        (task) =>
        {
            IsLoginOnProgress = false;
            loginBtn.interactable = true;          

            if(task.IsCompleted && !task.IsCanceled && !task.IsFaulted) //로그인에 성공했을 때
            {
                SceneManager.LoadScene("LobbyScene"); // 로비로 이동
            }

            else
            {
                SetPopUp("로그인에 실패했습니다.");
            } // 로그인이 실패했을 때
        }
        );
    }
   
    public void SendVerficationEmail() // 인증 메일을 보내는 메소드
    {
        user = firebaseAuth.CurrentUser;

        if (user != null)
        {
            user.SendEmailVerificationAsync().ContinueWithOnMainThread(stat => {
                if (stat.IsCanceled)
                {
                    SetPopUp("인증메일 전송이 취소되었습니다.");
                    return;
                }

                if (stat.IsFaulted)
                {
                    SetPopUp("인증메일 전송에 실패했습니다..");
                    return;
                }

                SetPopUp("인증메일을 전송했습니다.");
            }); 
        }

        else if(user == null) 
        {
            SetPopUp("user is null");
        }

        user = null;
    }

    public void SignUp() // 회원 가입 메소드
    {
        if(emailFieldSignUp.text.Length == 0) // email을 빈칸으로 썼을 때
        {
            SetPopUp("email을 확인해주세요.");
            return;
        }

        if(passwordFieldSignUp.text != passwordCheckField.text) // 비밀번호 확인이 불일치일 때
        {
            SetPopUp("비밀번호가 일치하지 않습니다.");
            return;
        }

        if (passwordFieldSignUp.text.Length < 6)
        {
            SetPopUp("비밀번호는 6글자 이상입니다.");
            return;
        }

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailFieldSignUp.text, passwordFieldSignUp.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                SetPopUp("회원가입이 취소되었습니다."); 
                return;
            }
            if (task.IsFaulted)
            {
                SetPopUp("회원가입이 실패습니다.");
                return;
            }

            SetPopUp("회원가입이 성공했습니다.");
        });
    }
    
    public void SetPopUp(string message) // 팝업창을 띄우는 함수
    {
        popUpText.text = message;
        popUp.SetActive(true);
    }
}
