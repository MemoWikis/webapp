import { createError, NuxtError } from 'nuxt/app';
import { ErrorCode } from './errorCodeEnum';
import { messages } from '../alert/alertStore'
import { getLastElement} from './utils'

export const createFromMessageKey = (messageKey: string) =>  {
    let statusCodeTemp: number;
    const lastWord =  getLastElement(messageKey.split('_'));
    console.log(lastWord);

    switch (lastWord) {
        case 'notFound':
            statusCodeTemp = ErrorCode.NotFound;
            break;
        case 'unauthorized':
            statusCodeTemp = ErrorCode.Unauthorized;
            break;
        case 'noRights':
            statusCodeTemp = ErrorCode.Unauthorized;
            break;
        default:
            statusCodeTemp = ErrorCode.Error;
    }    
    const error = createError({
    statusCode: statusCodeTemp,
    message: messages.getByCompositeKey(messageKey)
  });

  return error;
}  