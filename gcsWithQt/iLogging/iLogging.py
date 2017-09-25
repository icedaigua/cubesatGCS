import logging

def iLogging(path=""):
    logging.basicConfig(level=logging.DEBUG,
            format='%(asctime)s %(filename)s %(funcName)s[line:%(lineno)d] %(levelname)s %(message)s',
            datefmt='%a, %d %b %Y %H:%M:%S',
            filename=path+'myapp.log',
            filemode='a')

def iLogWarning(msg):
    logging.warning(msg)

def iLogDebug(msg):
    logging.debug(msg)

def iLogInfo(msg):
    logging.info(msg)

def iLogCritical(msg):
    logging.critical(msg)
    
if __name__ == '__main__':
    iLogging()
    logging.warning("Warning")
    logging.debug("Debug")
    logging.info("info")


