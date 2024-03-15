/* eslint-disable @typescript-eslint/require-await */
import { useState, type FC } from "react";
import { Link, useNavigate } from "react-router-dom";
import type { resetPasswordDto } from "../api/store/auth/interface";
import { useResetPasswordMutation } from "../api/store/auth/mutations";

const ResetPassword: FC = (): JSX.Element => {
	const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const handleEmailChange = (
        event: React.ChangeEvent<HTMLInputElement>
    ): void => {
        setEmail(event.target.value);
    };
    const resetMutation = useResetPasswordMutation();
    const handleSubmit = async (
        event: React.FormEvent<HTMLFormElement>
    ): Promise<void> => {
        event.preventDefault();

        const data: resetPasswordDto = {
            email,
        };
        resetMutation.mutate(data, {
            onSuccess() {
                navigate("/home");
            },
            onError(error) {
                console.error(error);
            },
        });
    };

    return (
        <div className=" bg-slate-200 min-h-screen overflow-hidden">

            <div className="max-w-lg mx-auto my-10 bg-white p-8 rounded-xl shadow shadow-slate-300">
                <h1 className="text-4xl font-medium">Reset password</h1>
                <p className="text-slate-500">Fill up the form to reset the password</p>
                <form onSubmit={handleSubmit} className="my-10">
                    <div className="flex flex-col space-y-5">
                        <label htmlFor="email">
                            <p className="font-medium text-slate-700 pb-2">Email address</p>
                            <input required id="email" name="email" type="email" onChange={handleEmailChange} className="w-full py-3 border border-slate-200 rounded-lg px-3 focus:outline-none focus:border-slate-500 hover:shadow" placeholder="Enter email address" />
                        </label>
                        <button className="w-full py-3 font-medium text-white bg--600 bg-black hover:bg-black rounded-lg border-black hover:shadow inline-flex space-x-2 items-center justify-center">
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
export default ResetPassword;