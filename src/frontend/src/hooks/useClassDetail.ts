import { useParams } from "react-router-dom";
import { classDetailQuery } from "../api/store/class/queries";

export default function useClassDetail(classId: string = "")  {
    if (classId=== "" && !classId) {
      const { id } = useParams();
        classId = id;
    }
    
    if (!classId) {
        throw new Error("id is required");
    }

    return classDetailQuery(classId);
}


