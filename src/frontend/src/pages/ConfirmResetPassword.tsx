import { useState, type FC, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import type { resetPasswordConfirmDto } from "../api/store/auth/interface";
import { userConfirmResetPasswordMutation } from "../api/store/auth/mutations";
import { Toast } from "flowbite-react";

const ConfirmResetPassword: FC = (): JSX.Element => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    password: "",
    confirmPassword: "",
  });

  // const [password, setPassword] = useState("");

  const [errMatch, setErrMatch] = useState(false);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    // eslint-disable-next-line @typescript-eslint/explicit-function-return-type
  ) => {
    const { id, value } = e.target;
    setForm((prevState) => ({
      ...prevState,
      [id]: value,
    }));
  };

  useEffect(() => {
    if (form.password !== form.confirmPassword) {
      setErrMatch(true);
    } else {
      setErrMatch(false);
    }
  }, [form.password, form.confirmPassword]);

  const confirmMutation = userConfirmResetPasswordMutation();
  const handleSubmit = async (
    event: React.FormEvent<HTMLFormElement>
    // eslint-disable-next-line @typescript-eslint/require-await
  ): Promise<void> => {
    event.preventDefault();

    const token = new URLSearchParams(window.location.search).get("code");
    const email = new URLSearchParams(window.location.search).get("email");

    if (!token || !email) {
      navigate("/not-found");
    }
    const data: resetPasswordConfirmDto = {
      email: email || "",
      code: token || "",
      password: form.password,
      confirmPassword: form.confirmPassword,
    };
    confirmMutation.mutate(data, {
      onSuccess() {
        navigate("/home");
      },
      onError(error) {
        console.log("[*] Code: ", token)
        // console.error("[*] Error: ", error?.response.data.title)
        console.error(error);
        Toast({
          title: "Fail",
          // description: error.response.data.title,
          // type: "error",
          duration: 500,
          // isClosable: true,
        })
      },
    });
  }

  return (
    <div className=" bg-slate-200 min-h-screen overflow-hidden">

      <div className="max-w-lg mx-auto my-10 bg-white p-8 rounded-xl shadow shadow-slate-300">
        <h1 className="text-4xl font-medium">Reset password</h1>
        <p className="text-slate-500">Fill up the form to reset the password</p>
        <form onSubmit={handleSubmit} className="my-10">
          <div className="flex flex-col space-y-5">
            <label htmlFor="password">
              <p className="font-medium text-slate-700 pb-2">Password</p>
              <input required id="password" name="password" type="password" onChange={handleChange} className="w-full py-3 border border-slate-200 rounded-lg px-3 focus:outline-none focus:border-slate-500 hover:shadow" placeholder="Enter password" />
            </label>
            <label htmlFor="password">
              <p className="font-medium text-slate-700 pb-2">Confirm password</p>
              <input required id="confirmPassword" name="confirm-password" type="password" onChange={handleChange} className="w-full py-3 border border-slate-200 rounded-lg px-3 focus:outline-none focus:border-slate-500 hover:shadow" placeholder="Enter confirm password" />
            </label>
            {errMatch && (
              <div className="flex flex-wrap -mx-3 mb-4">
                <div className="w-full px-3">
                  <div className="flex justify-between">
                    <p className="text-red-600">
                      Password does not match
                    </p>
                  </div>
                </div>
              </div>
            )}
            <button className="w-full py-3 font-medium text-white bg--600 bg-black hover:bg-black rounded-lg border-black hover:shadow inline-flex space-x-2 items-center justify-center ">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className="w-6 h-6">
                <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 5.25a3 3 0 013 3m3 0a6 6 0 01-7.029 5.912c-.563-.097-1.159.026-1.563.43L10.5 17.25H8.25v2.25H6v2.25H2.25v-2.818c0-.597.237-1.17.659-1.591l6.499-6.499c.404-.404.527-1 .43-1.563A6 6 0 1121.75 8.25z" />
              </svg>
              <span>Reset password</span>
            </button>
            <Link
              to="/sign-up"
              className="text-black font-medium inline-flex-right  space-x-1 items-center"
            >
              Register now!
            </Link>
          </div>
        </form>
      </div>
    </div>

  )
}
export default ConfirmResetPassword;