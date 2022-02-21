## Grammar rules
Where S is the starting non terminal, regular expressions are terminals

|Non terminals| Non terminals + Terminals                               |
|-------------|---------------------------------------------------------|
| S           | (INNERSCOPE* | METHOD*)+                                |
| VARIABLE    | NAME = ASSIGNMENT;                                      |
| INCREMENT   | NAME += ASSIGNMENT;                                     |
| DECREMENT   | NAME -= ASSIGNMENT;                                     |
| NAME        | [a-zA-Z][a-zA-Z0-9]*                                    |
| DIGIT       | [0-9]+                                                  |
| ASSIGNMENT  | NAME | DIGIT | METHOD-CALL                              |
| WHILE       | while NAME != 0: INNERSCOPE#                            |
| METHOD      | METHOD-HEAD INNERSCOPE return (ASSIGNTMENT | ε);        |
| METHOD-HEAD | TYPE NAME(ASSIGNTMENT | (ASSIGNMENT,)+ ASSIGNMENT | ε): |
| METHOD-CALL | NAME(ASSIGNTMENT | (ASSIGNMENT,)+ ASSIGNMENT | ε);      |
| INNERSCOPE  | (VARIABLE* | METHOD-CALL* | WHILE*)+                    |

---
## Example tokenizer:
This piece of code returns this "program stack" which is a tree of stackables
```py
x = 5;

num add(x, y):
    while a != 0:
        while b != 0:
            c = 5;
        #
        d = 5;
    #
    return z;
```

```
Scope:{
  Stack: {
    Variable token: {name: x, Assignment: {Value: 5}}
  }
  Methods: {
    Method token: {
      Header: {name: add return type: Num, parameters: [{Value: x}, {Value: y}]}
      Stack: {
        Return token: {Value: z}
        While token: {
          Header: {while target: {Value: a}}
          Stack: {
            Variable token: {name: d, Assignment: {Value: 5}}
            While token: {
              Header: {while target: {Value: b}}
              Stack: {
                Variable token: {name: c, Assignment: {Value: 5}}
              }
            }
          }
        }
      }
    }
  }
}
```


