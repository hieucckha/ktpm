/**
 * Joins classes together.
 * @param classes - List of classes to be joined.
 * @returns - Joined classes.
 */
const classNames = (...classes: string[]) => classes.filter(Boolean).join(" ");

export default classNames;
