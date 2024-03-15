import { FC } from "react";
import { ClassQuery } from "../../api/store/class/interface";
import { Link } from "react-router-dom";


const CardClass: FC<ClassQuery> = (data:ClassQuery) => {

return (
	<div className="max-w-sm bg-white border border-gray-200 rounded-lg shadow-lg shadow-gray-300/50 dark:bg-gray-800 dark:border-gray-700 hover:shadow-transparent cursor-pointer">
		<Link to={`/class/${data.id}/overview`}>
			<img
				className="rounded-t-lg"
				src="https://gstatic.com/classroom/themes/img_reachout.jpg"
			/>
		</Link>
		<div className="p-5">
			<Link to={`/class/${data.id}/overview`}>
				<h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
					{data.name}
				</h5>
			</Link>
			<p className="mb-3 font-normal text-gray-700 dark:text-gray-400">
				{data.description}
			</p>
		</div>
	</div>
);

}
export default CardClass;