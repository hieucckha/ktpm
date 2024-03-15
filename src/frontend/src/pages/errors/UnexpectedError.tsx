import { Result, ResultProps } from "antd";

interface UnexpetedErrorProps extends ResultProps {
	error?: Error;
}

function UnexpectedError({
	error,
	status,
	title,
	...rest
}: UnexpetedErrorProps) {
	return (
		<Result
			status={status ?? "500"}
			title={title ?? "500"}
			subTitle={error ? error.message : "Sorry, something went wrong."}
			{...rest}
		/>
	);
}

export default UnexpectedError;
