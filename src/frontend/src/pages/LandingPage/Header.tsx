import { useState, type FC, useEffect } from "react";
import { Link } from "react-router-dom";

const Header: FC = () => {
	const [top, setTop] = useState(true);

	// detect whether user has scrolled the page down by 10px
	useEffect(() => {
		const scrollHandler = (): void => {
			window.scrollY > 10 ? setTop(false) : setTop(true);
		};

		window.addEventListener("scroll", scrollHandler);

		return () => {
			window.removeEventListener("scroll", scrollHandler);
		};
	}, [top]);

	return (
		<header
			className={`fixed w-full z-30 md:bg-opacity-95 transition duration-300 ease-in-out ${
				!top && "bg-white shadow-lg"
			}`}
		>
			<div className="max-w-6xl mx-auto px-5 sm:px-6">
				<div className="flex items-center justify-between h-12 md:h-16">
					{/* Site branding */}
					<div className="flex-shrink-0 mr-4">
						{/* Logo */}
						<Link to="/" className="block" aria-label="Cruip">
							<img
								src="/icons/ssandwich.ico"
								alt="ssandwich_logo"
								className="w-8"
							/>
						</Link>
					</div>

					{/* Site navigation */}
					<nav className="flex flex-grow">
						<ul className="flex flex-grow justify-end flex-wrap items-center">
							<li>
								<Link
									to="/sign-in"
									className="font-medium text-gray-600 hover:text-gray-900 px-5 py-2 flex items-center transition duration-150 ease-in-out"
								>
									Sign in
								</Link>
							</li>

							<li>
								<Link
									to="/sign-up"
									className="btn-sm text-gray-200  hover:bg-gray-800 ml-3"
									style={{ backgroundColor: "#137333" }}
								>
									<span>Sign up</span>
									<svg
										className="w-3 h-3 fill-current text-gray-400 flex-shrink-0 ml-2 -mr-1"
										viewBox="0 0 12 12"
										xmlns="http://www.w3.org/2000/svg"
									>
										<path
											d="M11.707 5.293L7 .586 5.586 2l3 3H0v2h8.586l-3 3L7 11.414l4.707-4.707a1 1 0 000-1.414z"
											fillRule="nonzero"
										/>
									</svg>
								</Link>
							</li>
						</ul>
					</nav>
				</div>
			</div>
		</header>
	);
};

export default Header;
