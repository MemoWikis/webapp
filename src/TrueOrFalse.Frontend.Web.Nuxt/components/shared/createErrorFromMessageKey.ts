import { createError, NuxtError } from 'nuxt/app';
import { ErrorCode } from './errorCodeEnum';
import { messages } from '../alert/alertStore'

export const create = (messageKey: string) =>  {
     let statusCodeTemp: number;
    let statusMessage: string;
    
    switch (messageKey) {
        case 'NotFound':
            statusCodeTemp = ErrorCode.NotFound;
            
            break;
        case 'Unauthorized':
            statusCodeTemp = ErrorCode.Unauthorized;
            statusMessage = 'Unauthorized';
            break;
        case 'Error':
            statusCodeTemp = ErrorCode.Error;
            statusMessage = 'Internal Server Error';
            break;
        default:
            statusCodeTemp = ErrorCode.Error;
            statusMessage = 'Unknown Error';
    }    
    const error = createError({
    statusCode: statusCodeTemp,
    statusMessage: statusMessage = messages.getByCompositeKey(messageKey),
    message: statusMessage = messages.getByCompositeKey(messageKey)
  });

  return error;
}