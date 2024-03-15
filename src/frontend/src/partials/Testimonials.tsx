import type { FC } from "react";

const Testimonials: FC = () => (
	<section className="relative">
		{/* Illustration behind content */}
		<div
			className="absolute left-1/2 transform -translate-x-1/2 bottom-0 pointer-events-none -mb-32"
			aria-hidden="true"
		>
			<svg
				width="1760"
				height="518"
				viewBox="0 0 1760 518"
				xmlns="http://www.w3.org/2000/svg"
			>
				<defs>
					<linearGradient
						x1="50%"
						y1="0%"
						x2="50%"
						y2="100%"
						id="illustration-02"
					>
						<stop stopColor="#FFF" offset="0%" />
						<stop stopColor="#EAEAEA" offset="77.402%" />
						<stop stopColor="#DFDFDF" offset="100%" />
					</linearGradient>
				</defs>
				<g
					transform="translate(0 -3)"
					fill="url(#illustration-02)"
					fillRule="evenodd"
				>
					<circle cx="1630" cy="128" r="128" />
					<circle cx="178" cy="481" r="40" />
				</g>
			</svg>
		</div>

		<div className="max-w-6xl mx-auto px-4 sm:px-6">
			<div className="py-12 md:py-20">
				{/* Section header */}
				<div className="max-w-3xl mx-auto text-center pb-5">
					<h2 className="h2 mb-4">
						Bring all of your tools together with Google Workspace for Education
					</h2>
					<p className="text-xl text-gray-600" data-aos="zoom-y-out">
						Google Workspace for Education empowers your school community with
						easy-to-use tools that elevate teaching, learning, collaboration,
						and productivity – all on one secure platform.
					</p>
				</div>

				{/* Testimonials */}
				<div className="max-w-3xl mx-auto mt-20" data-aos="zoom-y-out">
					<div className="relative flex items-start border-2 border-gray-200 rounded bg-white">
						{/* Testimonial */}
						<div className="text-center px-12 py-8 pt-20 mx-4 md:mx-0">
							<div className="absolute top-0 -mt-8 left-1/2 transform -translate-x-1/2">
								<img
									className="relative rounded-full"
									src="/images/testimonial.jpg"
									width="96"
									height="96"
									alt="Testimonial 01"
								/>
							</div>
							<blockquote className="text-xl font-medium mb-4">
								“ I love this product and would recommend it to anyone. Could be
								not easier to use, and our multiple websites are wonderful. We
								get nice comments all the time. “
							</blockquote>
							<cite className="block font-bold text-lg not-italic mb-1">
								Suong Vo
							</cite>
							<div className="text-gray-600">
								<span>Co-Founder</span>{" "}
								<a className="text-blue-600 hover:underline" href="#0">
									@SomeSandwich
								</a>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</section>
);

export default Testimonials;
