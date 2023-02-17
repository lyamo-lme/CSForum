
export const request = async (pathUrl: string,method:"POST"|"GET"|"PUT"|"DELETE",headers?:any, variables?: any) => {
    return await fetch(pathUrl, {
        method: method,
        headers:headers,
        body: JSON.stringify({variables }),
    });
}